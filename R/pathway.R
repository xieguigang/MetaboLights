imports "Metabolon" from "MetaboLights";

require(igraph);

#' rendering the pathway map
#' 
const pathmaps = function(outputdir = "./", highlights = NULL) {
    let readJSON = function(name, what) {
        `data/Metabolon/${name}` 
        |> system.file(package = "MetaboLights")
        |> loadJSON(what)
        ;
    }
    let associations = readJSON("association_matrix.v6.json", 
        what = "association_matrix.v6");
    let graph_path = readJSON("metabolon-network.json", 
        what = "metabolon_network");
    let graph_render = Metabolon::as.render(
        network = graph_path, 
        association = associations);
    let graph = Metabolon::highlights(graph_render, highlight = highlights);

    svg(file = file.path(outputdir, "render.svg")) {
        plot(graph_render, highlight = highlights);
    }

    igraph::save.network(graph, file = outputdir, 
        properties = "*", 
        meta = igraph::metadata(
            title = "",
            description = "",
            creators = [],
            create_time = toString(now()),
            links = [],
            keywords = names(highlights)
        ));
}