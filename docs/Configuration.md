# Prometheus 配置

Prometheus 可以通过命令行和配置文件配置。命令行主要是配置不变的数据，如硬件设备指数等，如内存使用率，CPU 使用率，存储路径等），配置文件主要是就定义与收集作业以及它们所属的实例相关的东西，以及要加载的规则文件。

## 配置文件

指定加载配置的文件，使用可选项 `--config.file` 标识。方括号表示参数是可选的。对于非列表参数，该值被设置为指定的默认值。

关于内置已经定义好的的占位符的规则详见：https://prometheus.io/docs/prometheus/latest/configuration/configuration/#configuration-file。

度量重标记配置（`<metric_relabel_configs>`）：

报警重标记配置（`<alert_relabel_configs>`）：

报警管理配置（`<alertmanager_configs>`）：https://prometheus.io/docs/prometheus/latest/configuration/configuration/#alertmanager_config