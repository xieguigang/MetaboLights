require(multcompView);

anova_analysis = function(x) {
    class = as.factor(x$class);

    x[, "class"] = NULL;
    x[,"color"] = NULL;
    x[, "sample_name"] = NULL;

    sum = NULL;
    mean = NULL;
    F = NULL;
    P = NULL;
    tukey = NULL;
    my_colors = c(
        rgb(143,199,74,maxColorValue = 255),
        rgb(242,104,34,maxColorValue = 255),
        rgb(111,145,202,maxColorValue = 255)
    );

    for(xcms_id in colnames(x)) {
        data = data.frame(
            response = as.numeric(x[, xcms_id]),
            group = class
        );

        aov_model = lm(data$response ~ data$group);
        aov_model = aov(aov_model);
        aov_result = summary(aov_model);
        aov_result = as.data.frame(aov_result[[1]]);
        aov_result = aov_result[1,,drop = TRUE];
        TUKEY = TukeyHSD(x = aov_model, "data$group", conf.level = 0.95);
        tukey_result = as.data.frame(TUKEY$`data$group`);
        tukey_result = rownames(tukey_result)[tukey_result$`p adj` < 0.05];
        skip_plot =(aov_result$`F value` >= 0.05) || (length(tukey_result) == 0);

        tukey = append(tukey, paste(tukey_result, collapse="; "));
        sum = append(sum, aov_result$`Sum Sq`);
        mean = append(mean, aov_result$`Mean Sq`);
        F = append(F, aov_result$`F value`);
        P = append(P, aov_result$`Pr(>F)`);

        labels = label_df(TUKEY,"data$group");

        if (!skip_plot) {
            svg(filename = sprintf("%s.svg", xcms_id));
            boxplot(data$response ~ data$group,
                ylim=c(min(data$response), 1.125*max(data$response)),
                col = my_colors[as.numeric(as.factor(labels[, 1]))],
                ylab = "value", main = "");
            dev.off();
        }
    }

    aov = data.frame(
        `Sum Sq` = sum,
        `Mean Sq` = mean,
        `F value` = F,
        `Pr(>F)` = P,
        tukey_hsd = tukey,
        row.names = colnames(x) 
    );

    write.csv(aov, file = "anova.csv");
}

label_df = function(TUKEY,variable) {
    levels = TUKEY[[variable]][, 4];
    labels = data.frame(multcompLetters(levels)['Letters']);
    labels$group = rownames(labels);
    labels = labels[order(labels$group),];
    labels;
}