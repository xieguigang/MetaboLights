anova_analysis = function(x) {
    class = x$class;

    x[, "class"] = NULL;
    x[,"color"] = NULL;
    x[, "sample_name"] = NULL;

    response = NULL;
    group = NULL;
    features = ncol(x);

    for(i in 1:nrow(x)) {
        response = append(response, unlist(x[i,,drop = TRUE]));
        group = append(group, rep(class[i], features));
    }

    data = data.frame(
        response = response,
        group = as.factor(group)
    );

    aov_model = aov(response ~ group, data);
    aov_result = summary(aov_model);
    tukey_result = TukeyHSD(aov_model);
    tukey_df = as.data.frame(tukey_result$group);
    tukey_df$Comparison = factor(row.names(tukey_df))

    print(aov_result);
    print(tukey_result);

    ggplot(tukey_df, aes(x = Comparison, y = diff, color = `p adj` < 0.05)) +
    geom_point() +
    geom_errorbar(aes(ymin = lwr, ymax = upr), width = 0.2) +
    theme_minimal() +
    labs(x = "Group Comparison", y = "Mean Difference", color = "Significant") +
    theme(axis.text.x = element_text(angle = 45, hjust = 1)) +
    scale_color_manual(values = c("red", "black"), labels = c("Significant", "Not Significant"))
}