pca = function(x) {
    library(ggfortify) # 用于自动绘制prcomp对象的图
    library(ggplot2)   # 用于自定义绘图

    dir.create("pca");
    setwd("pca");

    # 假设你已经有了一个名为data的数据框，其中包含了你的代谢组学数据
    # data <- read.csv("path_to_your_data.csv", row.names = 1)
    # data <- x[, -c("sample_name","class","color")];
    data <- as.data.frame(x);
    data[, "sample_name"] = NULL;
    data[, "class"] = NULL;
    data[, "color"] = NULL;
    data_pca <- prcomp(data, scale. = TRUE, rank = 3) # 进行PCA分析，排除最后一列（物种）

    # 查看PCA结果
    summary(data_pca)

    # 将PCA结果转换为数据框，并添加物种信息
    pca_data <- as.data.frame(data_pca$x)
    pca_data$class <- x$class

    write.csv(pca_data, file = "pca.csv", row.names = TRUE);

    svg(filename = "pca.svg");

    # 使用ggplot2自定义绘图，并为每个species类别添加置信区间椭圆
    ggplot(pca_data, aes(x = PC1, y = PC2, color = class, group = class)) +
        geom_point() +  # 绘制点
        stat_ellipse(type = "norm", level = 0.95, geom = "polygon", alpha = 0.2, fill = NA) +  # 添加置信区间椭圆
        theme_minimal() +  # 使用简洁主题
        labs(color = 'class') +  # 添加图例标题
        ggtitle("PCA of Expression Data with Confidence Ellipses by Species") +  # 添加图表标题
        theme(legend.position = "bottom")  # 将图例放在底部

    dev.off();

    setwd("..");
}
