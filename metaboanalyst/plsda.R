plsda = function(x, oplsda = FALSE) {
    # 载入所需的R包
    library(ropls) # 用于PLS-DA分析
    library(ggplot2) # 用于自定义绘图

    # 假设你已经有了一个名为data的数据框，其中包含了你的代谢组学数据
    # data <- read.csv("path_to_your_data.csv", row.names = 1)

    # 由于iris数据集是连续的，我们需要将其转换为分类变量
    data = as.data.frame(x);
    data_class <- as.factor(data$class)
    data_pls <- NULL;

    data[, "color"] = NULL;
    data[, "sample_name"] = NULL;
    data[, "class"] = NULL;

    # 进行PLS-DA分析
    svg(filename = "ropls.svg");

    if (oplsda) {
        data_pls <- opls(data, y = data_class, predI = 3, orthoI = NA)
    } else {
        data_pls <- opls(data, y = data_class, predI = 3)
    }

    dev.off();

    if (oplsda) {
        # 提取PLS-DA得分
        plsda_scores <- as.data.frame(data_pls@orthoScoreMN);
        plsda_scores <- data.frame(
            Score1 = plsda_scores$o1,
            Score2 = plsda_scores$o2,
            Score3 = plsda_scores$o3
        );
    } else {
        # 提取PLS-DA得分
        plsda_scores <- as.data.frame(data_pls@scoreMN);
        plsda_scores <- data.frame(
            Score1 = plsda_scores$p1,
            Score2 = plsda_scores$p2,
            Score3 = plsda_scores$p3
        );
    }

    # 将PLS-DA得分和物种信息合并为一个数据框
    plsda_data <- data.frame(class = data_class, plsda_scores)

    svg(filename = "plsda.svg");

    # 使用ggplot2自定义绘图
    ggplot(plsda_data, aes(x = Score1, y = Score2, color = class, group = class)) +
        geom_point() +  # 绘制点
        stat_ellipse(type = "norm", level = 0.95, geom = "polygon", fill = NA, alpha = 0.2) +  # 添加置信区间椭圆
        theme_minimal() +  # 使用简洁主题
        labs(color = 'class') +  # 添加图例标题
        ggtitle("PLS-DA of Expression Data with Confidence Ellipses by Class") +  # 添加图表标题
        theme(legend.position = "bottom")  # 将图例放在底部

    dev.off();
}