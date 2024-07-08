require(MetaboLights)


let data = combine_sampleinfo(x = "G:\MetaboLights\data\Prostate_cancer_Benchmark\norm.csv",
                              sampleinfo = "G:\MetaboLights\data\Prostate_cancer_Benchmark\sampleinfo.csv")


write.csv(data, file = `${@dir}/norm.csv`)
