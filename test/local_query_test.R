require(MetaboLights);
require(Darwinism);

imports "repository" from "MetaboLights";
imports "memory_query" from "Darwinism";

setwd(@dir);

let repo = loadMetaEntries("./eb_eye_metabolights_complete.xml") |> as.metaSet |> experiments();
let pool = memory_query::load(repo);

pool = memory_query::fulltext(repo, "description");