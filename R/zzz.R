imports "repository" from "MetaboLights";
imports "MTBLSStudy" from "MetaboLights";

#' package startup
#' 
const .onLoad = function() {
	cat("----- MetaboLights -----\n");
    cat("
MetaboLights is a database for Metabolomics
experiments and derived information. The
database is cross-species, cross-technique
and covers metabolite structures and their
reference spectra as well as their biological
roles, locations and concentrations, and
experimental data from metabolic experiments.
");
    cat("\n");
}