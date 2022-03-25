imports "repository" from "MetaboLights";

#' package startup
#' 
const .onLoad = function() {
	cat("----- MetaboLights -----\n");
    cat("\n");
    cat("
MetaboLights is a database for Metabolomics \n
experiments and derived information. The \n
database is cross-species, cross-technique \n
and covers metabolite structures and their \n
reference spectra as well as their biological \n
roles, locations and concentrations, and \n
experimental data from metabolic experiments.");
}