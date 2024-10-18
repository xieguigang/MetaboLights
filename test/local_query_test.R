require(MetaboLights);
require(Darwinism);

imports "repository" from "MetaboLights";
imports "memory_query" from "Darwinism";

setwd(@dir);

let repo = loadMetaEntries("./eb_eye_metabolights_complete.xml") |> as.metaSet |> experiments();
let pool = memory_query::load(repo);

pool = pool |> memory_query::fulltext("protocols.mass_spectrometry")
            |> memory_query::fulltext("description")
            ;

let experiments = pool |> select(match_against("protocols.mass_spectrometry", "fisher"));

print(as.data.frame(experiments));