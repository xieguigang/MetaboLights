require(MetaboLights);

raw = "F:/eb-eye_metabolights_complete.xml"
|> loadMetaEntries()
|> as.metaSet()
;

experiments = raw |> experiments();
metabolites = raw |> metabolites();

print("get analysis experiments:");
print(length(experiments));
print("total metabolites:");
print(length(metabolites));

pause();