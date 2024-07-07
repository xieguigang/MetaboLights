#' Combine the matrix with sampleinfo
#' 
const combine_sampleinfo = function(x, sampleinfo) {
    x          <- coerce_dataframe(x);
    sampleinfo <- coerce_dataframe(sampleinfo); 

    print("view of the sampleinfo data:");
    print(sampleinfo, max.print = 6);
}