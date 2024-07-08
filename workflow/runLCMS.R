require(mzkit);
require(MetaboLights);

[@info "the file path to the rawdata matrix file, should be a csv excel table file 
        in format of features in rows and sample in columns."]
let raw_data = ?"--rawdata" || stop("a rawdata matrix must be provided!");

[@info "the file path to the sample information class data, this file should 
        contains the data fields: 
        
        1. ID - the sample id, which will be used for reference to the sample data inside the expression matrix;	
        2. injectionOrder - the injection order in the LCMS experiments, this information maybe usefull for data normalization;	
        3. batch - the large panel batch number for the corresponding sample;	
        4. sample_name - the display name of the samples;	
        5. sample_info - the sample class or sample group name;	
        6. color - color of the corresponding sample;	
        7. shape - the shape name for draw the scatter of the sample data
"]
let sampleinfo = ?"--sampleinfo" || stop("the sampleinfo metadata must be provided!");

[@info "a directory path for export the analysis result."]
let workdir = ?"--export_dir" || file.path(dirname(raw_data), "LCMS_analysis");

# rawdata pre-processing
# impute and normalization
sampleinfo <- read.csv(sampleinfo, row.names = NULL, check.names = FALSE);
# binary/csv/txt
raw_data  <- read.xcms_peaks(raw_data, tsv = file.ext(raw_data) != "csv");
raw_data  <- mzkit::preprocessing_expression(raw_data, 
    sampleinfo = sampleinfo, 
    factor = 1e8, missing = 0.3
);

let processed_raw = file.path(workdir, "data.csv");
let run = function() {
    let x = read.csv(processed_raw, row.names = 1, check.names = FALSE);

    setwd(workdir);

    pca(x);
    plsda(x);
    oplsda(x);     
}
let deps = system.file("metaboanalyst/readme.txt", package = "MetaboLights") 
|> dirname() 
|> list.files(pattern = "*.R", 
    recursive = TRUE)
;

# combine data with sample metadata
raw_data 
|> combine_sampleinfo(sampleinfo)
|> write.csv(x, file = processed_raw, 
    row.names = TRUE);

REnv::rlang_interop(run, source = deps, 
    debug = TRUE);

