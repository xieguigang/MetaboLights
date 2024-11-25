require(MetaboLights);
require(Darwinism);
require(xlsx);

imports "repository" from "MetaboLights";
imports "memory_query" from "Darwinism";

setwd(@dir);

let repo = loadMetaEntries("./eb_eye_metabolights_complete.xml") |> as.metaSet |> experiments();
let pool = memory_query::load(repo);

pool = pool |> memory_query::fulltext("protocols.mass_spectrometry")
            |> memory_query::fulltext("description")
            ;

let serum = pool |> select(match_against("protocols.mass_spectrometry", "thermo fisher"), match_against("description", "Serum"));
let plasma = pool |> select(match_against("protocols.mass_spectrometry", "thermo fisher"), match_against("description", "plasma"));
let blood = pool |> select(match_against("protocols.mass_spectrometry", "thermo fisher"), match_against("description", "blood"));
let experiments = as.data.frame([...serum, ...plasma, ...blood]);

print(experiments);

write.csv(experiments, file = "./experiments_filter.csv");
write.xlsx(experiments, file = "./experiments_filter.xlsx");
writeLines(rownames(experiments), con = "./id.txt");


let thermo_fisher_ms = pool |> select(match_against("protocols.mass_spectrometry", "thermo fisher")) |> as.data.frame();

write.xlsx(thermo_fisher_ms, file = "./thermo_fisher_ms.xlsx");
write.csv(thermo_fisher_ms, file = "./thermo_fisher_ms.csv");
