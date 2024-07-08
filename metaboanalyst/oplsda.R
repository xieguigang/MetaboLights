oplsda = function(x) {
    workdir = getwd();
    class = unique(x$class);
    sample_class = x$class;

    # opls-da only works for two group
    for(class_name in class) {
        v <- as.character(sample_class);
        v[v != class_name] = "Other";
        x[,"class"] = v;

        dir.create(class_name);
        setwd(class_name);
        plsda(x, oplsda = TRUE);
        setwd(workdir);
    }
}