require(MetaboLights);
require(JSON);

let metadata = MTBLSStudy::read.study_source(file = file.path(@dir, "s_mtbls330.txt"));

for(sample in metadata) {
    str(as.list(sample));
}

metadata
|> JSON::json_encode()
|> writeLines(con = file.path(@dir, "s_mtbls330.json"))
;

metadata 
|> sampleinfo("Characteristics", "Organism")
|> write.csv(file = file.path(@dir, "sampleinfo-organism.csv"))
;
