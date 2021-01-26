# 联合

Federation 允许一个普罗米修斯服务器从另一个普罗米修斯服务器上收集选定的时间序列。

## 使用案例

 通常，它用于实现可扩展的 Prometheus 监视设置，或者将相关指标从一个服务的 Prometheus 转到另一个服务中。

### 分层联合

### 跨服务联合

## 配置联合

```yaml
scrape_configs:
  - job_name: 'federate'
    scrape_interval: 15s

    honor_labels: true
    metrics_path: '/federate'

    params:
      'match[]':
        - '{job="prometheus"}'
        - '{__name__=~"job:.*"}'

    static_configs:
      - targets:
        - 'source-prometheus-1:9090'
        - 'source-prometheus-2:9090'
        - 'source-prometheus-3:9090'
```