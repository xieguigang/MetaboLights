require(MetaboLights);

raw = "E:/eb-eye_metabolights_complete.xml"
|> loadMetaEntries()
|> as.metaSet()
;

let exprs = raw |> experiments();
let metabos = raw |> metabolites(mzkit = TRUE);

print("get analysis experiments:");
print(length(exprs));
print("total metabolites:");
print(length(metabos));

pause();