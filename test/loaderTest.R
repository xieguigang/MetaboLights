require(MetaboLights);

raw = "E:/eb-eye_metabolights_complete.xml"
|> loadMetaEntries()
|> as.metaSet()
;

let exprs = raw |> experiments();
let metabos = raw |> metabolites();

print("get analysis experiments:");
print(length(exprs));
print("total metabolites:");
print(length(metabos));

setwd(@dir);

write.csv(metabos, file = "./MetaboLights.csv");