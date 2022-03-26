require(JSON);
require(mzkit);

imports "formula" from "mzkit";

setwd(@dir);

data = "./metabolites.json"
|> readText()
|> json_decode()
;

formula = sapply(data, x -> x$formula);

data = data.frame(

		id = names(data),
		name = sapply(data, x -> x$name),
		formula = formula,
		exactMass = formula::eval(formula),
		inchi = sapply(data, x -> x$inchi)	

);

write.csv(data, file = "./Metabolights.csv", row.names = FALSE);