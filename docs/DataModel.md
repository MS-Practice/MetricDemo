# 数据模型

Prometheus 是把所有数据以时间序列方式存储的：具有时间戳的值流属于同一个度量和同一组标记的维度。在存储时间序列方面，Prometheus 会生成临时派生的时间做为查询的结果。

## 度量名称（names）和度量标签（labels）

每个时间序列都由它的度量名称和称为标签的可选键-值对唯一标识。

度量名称指定要度量的系统的一般特性（如 `http_requests_total` 标识总的 HTTP 接收数）。它可以包含ASCII字母和数字，以及下划线和冒号。它必须匹配正则 `[a-zA-Z_:][a-zA-Z0-9_:]*`

冒号保留给用户自定义的记录规则。它们不应被出口商（exporters）或直接测量工具（direct instrumentation）使用。

标签支持 Prometheus 的维度数据模型：相同度量名称的任何给定标签组合都标识该度量的特定维度的实例化（例如：所有使用 POST 方法的 `api/tracks` 的 HTTP 请求处理程序）。查询语言支持基于这些维度过滤和聚合。更改任何标签值，包括增加和删除标签都会生成新的时间序列。

标签（Labels）名称可以包含 ASCII 字母、数字以及下划线。它们必须是能匹配这个正则式：`[a-zA-Z_][a-zA-Z0-9_]*`。标签名称可以用 `_` 保留在内部使用。

标签值也能包含 Unicode 字符集。

一个不存在值得空标签等同于标签不存在。

## 样本

样本形成了实际时间序列数据。每个样本都由下面两部分构成：

- 一个 float64 值
- 一个毫秒精度的时间戳

## 符号 Notation

给定一个度量名称和标签，时间序列会经常性的使用符号来标识：

```
<metric name>{<label name>=<label value>, ...}
```

例如，度量名称为 `api_http_requests_total` 的一个时间序列和一个 `method="POST"` 以及 `handler="/messages"` 的标签写成如下形式：

```
api_http_requests_total{method="POST", handler="/messages"}
```

这在 [OpenTSDB](http://opentsdb.net/) 使用的符号是一致的。