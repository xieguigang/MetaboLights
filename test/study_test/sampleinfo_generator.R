require(MetaboLights);
require(JSON);

let maf_source = "\\192.168.1.254\backup3\项目以外内容\human_reference_metabolome\benchmark\MTBLS6039\s_MTBLS6039.txt";
let metadata = MTBLSStudy::read.study_source(file =  maf_source);
let workdir = "\\192.168.1.254\backup3\项目以外内容\human_reference_metabolome\benchmark\MTBLS6039\FILES\RAW_FILES";

metadata
|> JSON::json_encode()
|> writeLines(con = file.path(workdir , "metadata.json"))
;

metadata 
|> sampleinfo("factor", "Cohort")
|> write.csv(file = file.path(workdir, "sampleinfo.csv"))
;
