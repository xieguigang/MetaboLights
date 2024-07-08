anova = function(x) {
    class = unique(x$class);
    aov_formula = as.formula(paste("response ~", paste(class, collapse = " * ")));
    aov_model = aov(aov_formula, x);
    aov_result = summary(aov_model);
    tukey_result = TukeyHSD(aov_model);

    print(aov_result);
    print(tukey_result);

    plot(tukey_result);

    ggplot(data = as.data.frame(tukey_result$`factor1:factor2`), aes(x = Df, y = diff, color = p adj < 0.05)) +
        geom_point() +
        geom_errorbar(aes(ymin = lwr, ymax = upr), width = 0.2) +
        theme_minimal() +
        labs(x = "Group Comparison", y = "Mean Difference", color = "Significant")
}