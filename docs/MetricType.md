# 度量类型（Metric Type）

Prometheus 客户端库提供了四种核心度量类型。它们目前仅在客户端库（以支持针对特定类型的使用而定制的 api）和有线协议（wire protocol）中有所区别。Prometheus 服务器还没有使用类型信息，并将所有数据扁平化为未输入类型的时间序列。这在未来版本中会有所改动。

## 计数器 Counter

counter 是一个累计度量，表示一个单调递增的计数器，计数器的值只能递增或重置为零。例如，你可以使用计数器来表示当前正运行的保留请求数，完成了多少任务，或出错的任务的数量。

**不要使用计数器来导出值，这会导致递减。**例如，当前正在运行的进程的数量不要用 counter，而是使用计量器（gauge）

## 计量器 Gauge

Gauge 表示一个数字值，代表能任意增加或减少。

Gauge 度量值是特定用在如测量温度和当前内存使用率，这种有可能上升或下降，如并发请求的数量。

## 直方图 Histogram

直方图对观察结果（通常是请求持续时间或响应大小）进行抽样，并在可配置的 buckets 中对它们进行计数。它还提供了所有观察值的总和。

一个基本的度量名称为 `<basename>` 的图表在收集期间能导出多个时间序列：

- 观察的 buckets 的累计计数器，导出为 `<basename>_bucket{le="<upper inclusivebound>"}`
- 所有观察的值的总和，导出为 `<basename>_sum` 导出
- 处于观察的事件的数量，导出为 `<basename>_count` （与上面的 `<basename>_bucket{le="+Inf"}` 完全一样）导出

使用 [`histogram_quantile()`](https://prometheus.io/docs/prometheus/latest/querying/functions/#histogram_quantile) 函数从直方图或事件聚合的直方图计算分位数（quantiles）。直方图也适合计算 [Apdex](https://en.wikipedia.org/wiki/Apdex) 分数。当操作 bukects 时，直方图是累加的。

## 摘要 Summary

与直方图类似，摘要对观察结果进行抽样（通常是请求持续时间和响应大小）。虽然它还提供了观察总数和所有观察值的总和，但它在滑动时间窗口上计算可配置的分位数。

基本度量名称为 `<basename>` 的摘要在收集期间暴露多个时间序列：

- 观测事件的 φ-分位数 (0≤φ≤1) 流，暴露为`<basename> {分位数="<φ>"}`
- 所有观察的值的总和，暴露为 `<basename>_sum`
- 观察的事件数，暴露为 `<basename>_count`

