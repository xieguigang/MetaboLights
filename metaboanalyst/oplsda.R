oplsda = function(x) {
    workdir = getwd();
    class = unique(x$class);
    sample_class = x$class;

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