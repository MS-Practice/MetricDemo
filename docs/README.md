# 导航

- [术语](GLOSSARY.md)
- [数据模型](DataModel.md)
- [度量类型](MetricType.md)
- [Job 与示例](Jobs-And-Instances.md)
- [配置](Configuration.md)
- [存储](Storage.md)
- [联合](Federation.md)

# 参考资料

- https://prometheus.io/docs/introduction/overview/

# Prometheus

## 特征

- 多维度的数据模型，时间序列数据（time series data）是通过度量名称以及 key/value 键值对来标识
- PromQL，一个灵活的查询语句
- 无依赖的分布式存储；单个服务节点是自治的
- 时间序列的收集通过在 HTTP 的 pull 上发生的
- 通过中间网关来支持推送时间序列
- 目标是通过服务发现或静态配置覆盖的
- 支持多个模型的图标以及仪表盘

## 组件

Prometheus 生态系统是由多个组件构成的，主要有下面可选的：

- 主要的是 Prometheus 服务器是收集和存储时间序列数据的
- 应用程序指令代码的客户短库
- 支持一个短期的推送官网的 job
- 专用的导出出口，如 HAProxy、StatsD、Graphite 等
- 一个警告处理程序来处理警告
- 各种支持工具

大多数 Prometheus 的工具都是用 Go 写的，这样会很容易生成二进制来部署。

## 架构

下面的图标分析了 Prometheus 的架构一些生态系统组件：

![](../asserts/architecture.png)

## 何时合适用它

Prometheus 可以很好地记录所有纯粹的数字时间序列。它既适合以机器为中心的监视，也适合高度动态的面向服务架构的监视。在微服务的世界里，它支持多维度的数据集合以及数据的查询。

Prometheus 被设计为可靠的，在出现故障时，你可以使用本系统快速的诊断稳态。每个 Prometheus 服务器都是独立的，不依赖于网络存储或其它远程服务。在你架构有受损部分的时候，你可以依赖它。并且你不需要设置大量的基础设施来使用它。

## 何时不合适

Prometheus 值的可靠性。你可以实时展示你系统的那些数据，即使是在故障条件下。如果你需要 100% 的精确度，比如请求计费，那么 Prometheus 就不是一个好选择，因为收集的数据不够详细和完整。在这种情况下你最好使用其它系统来收集和分析数据计费，并且 Prometheus 会监视你系统的其它部分。

## 安装

https://prometheus.io/docs/introduction/first_steps/#downloading-prometheus

## 配置 Prometheus 监控目标样本（sample targets）

我们现在这些要监控的目标样本：http://localhost:8080/metrics、http://localhost:8081/metrics、http://localhost:8082/metrics。

现在我们要配置收集这些目标样本。我们先把这三个终结点分组到一个作业里，称为 `node`。假设我们两个是生产节点，还有一个是测试节点。为了在 Prometheus 中模拟这一点，我们可以在单个任务中添加几组端点，并为每组目标添加额外的标签。在这个例子中，我们添加组 `group="production"` 标签到第一组目标中，给第二组添加 `group="canary"`。

为了达到目的，我们在 `prometheus.yml` 中添加定义如下 `scrape_configs` 节点，并重启 Prometheus 实例：

```yaml
scrape_configs:
  - job_name:       'node'

    # Override the global default and scrape targets from this job every 5 seconds.
    scrape_interval: 5s

    static_configs:
      - targets: ['localhost:8080', 'localhost:8081']
        labels:
          group: 'production'

      - targets: ['localhost:8082']
        labels:
          group: 'canary'
```

转到表达式浏览器，检查 Prometheus 现在是否有关于这些样本终结点暴露的时间序列的信息，例如 node_cpu_seconds_total。

## 配置将收集的数据聚合到新的时间序列的规则

虽然在我们的示例中不是问题，但聚合在数千个时间序列上的查询在特别计算时可能会变慢。为了提高效率，Prometheus 可以通过配置记录规则（recording rules）将表达式预先录制到新的持久时间序列中。假设我们想记录 5 分钟的时间窗口内每个实例所有 cpu 上的平均 cpu 时间（node_cpu_seconds_total）的每秒速率(但保留 `job`、`instance`和 `mode` 维度)。我们可以这样配置：

```yaml
avg by (job, instance, mode) (rate(node_cpu_seconds_total[5m]))
```

试着把这个表达式画出来。

将这个表达式产生的时间序列记录到一个名为 `job_instance_mode:node_cpu_seconds:avg_rate5m` 的新度量中，按照下面的规则创建一个文件以 `prometheus.rules.yml` 保存：

```yaml
groups:
- name: cpu-node
  rules:
  - record: job_instance_mode:node_cpu_seconds:avg_rate5m
    expr: avg by (job, instance, mode) (rate(node_cpu_seconds_total[5m]))
```

为了让 Prometheus 应用新的规则，添加 `rule_files` 语句到 `prometheus.yml` 中。配置文件内容如下：

```yaml
global:
  scrape_interval:     15s # By default, scrape targets every 15 seconds.
  evaluation_interval: 15s # Evaluate rules every 15 seconds.

  # Attach these extra labels to all timeseries collected by this Prometheus instance.
  external_labels:
    monitor: 'codelab-monitor'

rule_files:
  - 'prometheus.rules.yml'

scrape_configs:
  - job_name: 'prometheus'

    # Override the global default and scrape targets from this job every 5 seconds.
    scrape_interval: 5s

    static_configs:
      - targets: ['localhost:9090']

  - job_name:       'node'

    # Override the global default and scrape targets from this job every 5 seconds.
    scrape_interval: 5s

    static_configs:
      - targets: ['localhost:8080', 'localhost:8081']
        labels:
          group: 'production'

      - targets: ['localhost:8082']
        labels:
          group: 'canary'
```

重启服务，并通过表达式浏览器查询或绘图来验证度量名称为 `job_instance_mode:node_cpu_seconds:avg_rate5m` 新的时间序列是否可用。