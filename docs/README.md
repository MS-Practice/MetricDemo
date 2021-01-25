# 导航

- [术语](GLOSSARY.md)

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

