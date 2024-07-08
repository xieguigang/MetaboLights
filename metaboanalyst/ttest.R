ttest = function(x) {
    class = unique(x$class);
    check = list();

    for(a in class) {
        for(b in class) {
            if (a != b) {
                key = c(a,b);
                key = key[order(key)];
                key = paste(key, collapse = " vs ");

                if (!any(key == names(check))) {
                    check[[key]] = c(a,b);
                    ttest_group(x, a,b);
                }
            }
        }
    }
}

ttest_group = function(x, a, b) {
    workdir = paste(c(a,b), collapse = " vs ");
    dir.create(workdir);

    i = a == x$class;
    j = b == x$class;
    i = x[i, ];
    j = x[j, ];
    i[, "class"] = NULL;
    j[, "class"] = NULL;
    i[, "color"] = NULL;
    j[, "color"] = NULL;
    i[, "sample_name"] = NULL;
    j[, "sample_name"] = NULL;
    i = t(i);
    j = t(j);
    mean_a = rowMeans(i);
    mean_b = rowMeans(j);
    sd_a = apply(i,1, sd);
    sd_b = apply(j,1,sd);
    foldchange = mean_a / mean_b;
    t = lapply(1:nrow(i), function(offset) {
        v1 = unlist(i[offset,,drop = TRUE]);
        v2 = unlist(j[offset,, drop = TRUE]);
        t.test(v1,v2, alternative = "two.sided" );
    });
    pvalue = sapply(t, function(x) x$p.value);
    t = sapply(t, function(x) x$statistic);
    t = data.frame(
        mean_a, mean_b, sd_a, sd_b,foldchange, log(foldchange,base =2),t,pvalue, 
        row.names = rownames(i)
    );
    
    colnames(t) = c(
        sprintf("meanOf_%s", a), sprintf("meanOf_%s", b),
        sprintf("sdOf_%s", a), sprintf("sdOf_%s", b),
        "foldchange", "log2FC","t","p.value"
    );

    write.csv(t, file = file.path(workdir, "ttest.csv"), row.names = TRUE);

    svg(filename = file.path(workdir, "volcano.svg"));
    volcano(t); 
    dev.off();
}

volcano = function(t, log_cutoff = 2, pval_cutoff = 0.05) {
    t[, "log10"] = -log10(t$p.value);
    t[, "sig"] = ifelse(t$log2FC > log_cutoff & t$p.value < pval_cutoff, "Upregulated",
                ifelse(t$log2FC < -log_cutoff & t$p.value < pval_cutoff, "Downregulated", "Not Significant"));

    ggplot(t, aes(x=log2FC, y=log10, color=sig)) +
        geom_point() +
        theme_minimal() +
        scale_color_manual(values=c("blue", "red", "grey"), labels=c("Upregulated", "Downregulated", "Not Significant")) +
        geom_vline(xintercept=c(-log_cutoff, log_cutoff), linetype="dashed") +
        geom_hline(yintercept=-log10(pval_cutoff), linetype="dashed") +
        labs(x="log2 Fold Change", y="-log10(p value)", color="Significance") +
        theme(legend.position="right")
}