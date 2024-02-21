require(MetaboLights);

let maf = load.csv(file = file.path(@dir, "m_mtbls330_NEG_Tomato_mass_spectrometry_v2_maf.tsv"), tsv = TRUE, type = "MTBLS_maf");

write.csv(maf, file = file.path(@dir, "metabolites.csv"));