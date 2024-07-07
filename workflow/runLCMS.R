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


# combine data with sample metadata
raw_data 
|> combine_sampleinfo(sampleinfo)
|> write.csv(x, file.path(workdir, "data.csv"), 
    row.names = TRUE);

