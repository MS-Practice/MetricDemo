# 作业（Jobs）与示例（Instances）

在 Prometheus 的术语中，可以收集的端点称为实例（Instance），通常对应于单个进程。具有相同目的的实例集合，例如为可伸缩性或可靠性而重复的过程，称为作业（job）。

例如，一个 API 服务作业有四个复制实例：

- Job：api-server
  - 实例1：`1.2.3.4:5670`
  - 实例2：`1.2.3.4:5671`
  - 实例3：`1.2.3.4:5672`
  - 实例4：`1.2.3.4:5673`

## 自动生成时间序列和标签

当 Prometheus 收集一个目标 target，它会自动在那些收集时间序列的对象（target）附加一些标签，用来标识那些收集的对象：

- `job`：配置 target 的作业名称
- `instance`：收集的目标 URL 的 `<host>:<port>` 的部分

如果这些标签已经存在于收集的数据中，那么行为取决于 `honor_labels` 配置选项，关于配置可查看 [收集配置文档](Configuration.md)。

每个收集的实例，Prometheus 在下面的时间序列中存储样本：

- `up{job="<job-name>", instance="<instance-id>"}`：`1` 表示实例是健康的，如是可查询，`0` 表示收集失败。
- `scrape_duration_seconds{job="<job-name>", instance="<instance-id>"}`：收集的持续时间。
- `scrape_samples_post_metric_relabeling{job="<job-name>", instance="<instance-id>"}`：在度量监控使用重新标签剩余的样本数量
- `scrape_samples_scraped{job="<job-name>", instance="<instance-id>"}`：被收集的目标暴露的样本数量
- `scrape_series_added{job="<job-name>", instance="<instance-id>"}`：在这个收集中新的序列的一个近似值，在版本 v2.10 才有

`up` 时间序列对于实例可用性监视非常有用。

