# Prometheus 术语

## 警报 Alert

## 警报管理器

## 网桥 Bridge

## 客户端库

## 收集器 Collector

## 直接测量工具 Direct Instrumentation

## 终结点

## 出口商 Exporter

## 示例 Instance

## 任务 Job

## 通知 Notification

## Prometheus

## PromQL



## PushGetway

 Pushgateway 保存来自批处理作业的最新指标推送。这使得 Prometheus 公司可以在他们终止后，收回他们的度量标准。

## Remote Read

远程读取是 Prometheus 的一个特性，它允许从其他系统(如长期存储)透明地读取时间序列，作为查询的一部分。

## Remote Read Adapter

并不是所有系统都支持远程读入。一个远程读入适配器位于 Prometheus 和其它系统之间，在它们之间转换时间序列请求和响应。

## Remote Read Endpoint

远程读取终结点是 Prometheus 在进行远程读取时要与之对话的对象。

## Remote Write

远程写是 Prometheus 的一个特征，它允许在正在途中将摄入的样本发送到其他系统，比如长期存储系统。

## Remote Write Adapter

并不是所有系统都直接支持远程写入。一个远程写适配器位于 Prometheus 和其它系统之间，**将远程写入中的示例转换为另一个系统可以理解的格式。**

## Remote Write Endpoint

 远程写端点是 Prometheus 在进行远程写操作时使用的对象

## 样本 Sample

样本是时间序列中某个时间点的单个值

## 静音 Silence

Alertmanager 中的“静音”功能可以防止将标签与“静音”匹配的警报包含在通知中。

## 目标 Target

Target 就是要收集的对象定义。例如要应用什么标签，连接需要验证，或者定义如何收集的其它信息。

