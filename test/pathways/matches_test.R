require(MetaboLights);

let data_input = read.csv("D:\\metabolites.csv", row.names = 1, check.name = FALSE);

print(data_input);

let data = pathway_metabolites(data_input$cas);

print(data);

