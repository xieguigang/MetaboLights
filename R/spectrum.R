#' Workflow function for extract the reference spectrum
#' 
#' @param maf a character vector of the file path to the maf annotation result table tsv files.
#' @param rawfiles a character vector of the raw data files, should be in mzpack data format!
#' 
const extract_spectra = function(maf, rawfiles, outputdir = "./") {
    rawfiles <- as.list(rawfiles, names = basename(rawfiles));
    maf <- lapply(maf, path -> load.csv(
        file = path, 
        tsv = TRUE, type = "MTBLS_maf"));
    maf <- unlist(maf);

    write.csv(maf, file = file.path(outputdir, "all_maf.csv"));

    maf <- as.data.frame(maf);

    print("previews of the annotation result table:");
    print(maf, max.print = 6);

    
}