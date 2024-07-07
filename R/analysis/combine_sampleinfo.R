#' Combine the matrix with sampleinfo
#' 
const combine_sampleinfo = function(x, sampleinfo) {
    x          <- coerce_dataframe(x);
    sampleinfo <- coerce_dataframe(sampleinfo); 

    print("view of the sampleinfo data:");
    print(sampleinfo, max.print = 6);

    x <- t(x);

    # assign sample name and sample class
    sampleinfo <- sampleinfo[rownames(x), ];
    sampleinfo <- data.frame(sample_name = sampleinfo$sample_name,
        class = sampleinfo$sample_info,
        color = sampleinfo$color,
        row.names = rownames(sampleinfo));
    # combine the expression matrix with sampleinfo
    x <- cbind(sampleinfo, x);

    x;
}