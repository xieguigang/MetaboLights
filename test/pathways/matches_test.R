require(MetaboLights);

let data_input = read.csv("D:\\metabolites.csv", row.names = 1, check.name = FALSE);

rownames(data_input) = `metabo_${rownames(data_input)}`;

print(data_input);

let data = pathway_metabolites(data_input$cas);

rownames(data) = rownames(data_input); 

print(data);

data = cbind(data_input, data);

write.csv(data, file = "D:/metabolite_matches_cas.csv", row.names = TRUE);

