pca = function(x) {
    library(ggfortify) # 用于自动绘制prcomp对象的图
    library(ggplot2)   # 用于自定义绘图

    # 假设你已经有了一个名为data的数据框，其中包含了你的代谢组学数据
    # data <- read.csv("path_to_your_data.csv", row.names = 1)
    data <- x[, -c("sample_name","class","color")];
    data_pca <- prcomp(data, scale. = TRUE, rank = 3) # 进行PCA分析，排除最后一列（物种）

    # 查看PCA结果
    summary(data_pca)

    # 使用ggfortify包自动绘制PCA图
    autoplot(data_pca, data = x, colour = 'color', label = TRUE, shape = FALSE)

    # 或者使用ggplot2自定义绘图
    pca_data <- as.data.frame(data_pca$x) # 转换PCA结果为数据框
    pca_data$class <- x$class      # 添加物种信息列

    ggplot(pca_data, aes(x = PC1, y = PC2, color = class)) +
    geom_point() +  # 绘制点
    theme_minimal() +  # 使用简洁主题
    labs(color = 'Sample Class') +  # 添加图例标题
    ggtitle("PCA of Expression Data") +  # 添加图表标题
    theme(legend.position = "bottom")  # 将图例放在底部
}
