imports "Metabolon" from "MetaboLights";

require(igraph);

let readJSON = function(name, what) {
    `data/Metabolon/${name}` 
    |> system.file(package = "MetaboLights")
    |> loadJSON(what)
    ;
}

#' rendering the pathway map
#' 
const pathmaps = function(outputdir = "./", highlights = NULL, default.fill_color = "lightgray") {
    let associations = readJSON("association_matrix.v6.json", 
        what = "association_matrix.v6");
    let graph_path = readJSON("metabolon-network.json", 
        what = "metabolon_network");
    let graph_render = Metabolon::as.render(
        network = graph_path, 
        association = associations);
    let graph = Metabolon::highlights(graph_render, highlight = highlights);

    svg(file = file.path(outputdir, "render.svg")) {
        plot(graph_render, highlight = highlights, 
            default.fill = default.fill_color);
    }

    igraph::save.network(graph, file = outputdir, 
        properties = "*", 
        meta = igraph::metadata(
            title = "Map of metabolic pathways with associated metabolites and genes",
            description = "metabolic pathways highlights",
            creators = ["Maria Ulmer", "Johannes Raffler", "xie.guigang@gcmodeller.org"],
            create_time = toString(now()),
            links = ["https://omicscience.org/", "https://mzkit.org/", "https://gcmodeller.org/", "https://rsharp.net/"],
            keywords = names(highlights)
        ));
}

const pathway_metabolites = function(cas_id) {
    let associations = readJSON("association_matrix.v6.json", 
        what = "association_matrix.v6");
    let matches = matches_cas(cas_id, associations);

    data.frame(
        cas_id = cas_id,
        name = [matches]::name,
        superpathway = [matches]::superpathway,
        subpathway = [matches]::subpathway,
        mass      = [matches]::mass,
        ri       = [matches]::ri,
        pubchem  = [matches]::pubchem,
        chemspider  = [matches]::chemspider,
        hmdb  = [matches]::hmdb,
        kegg  = [matches]::kegg  
    );
}