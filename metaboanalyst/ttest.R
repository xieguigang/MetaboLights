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
    t = sapply(1:nrow(i), function(offset) {
        v1 = unlist(i[offset,,drop = TRUE]);
        v2 = unlist(j[offset,, drop = TRUE]);
        t.test(v1,v2, alternative = "two.sided" );
    });
    pvalue = sapply(t, function(x) x@p.value);
    t = sapply(t, function(x) x@statistic);
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

    
}