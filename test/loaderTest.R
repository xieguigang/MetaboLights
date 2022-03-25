require(MetaboLights);

raw = "F:/eb-eye_metabolights_complete.xml"
|> loadMetaEntries()
|> as.metaSet()
;

experiments = raw |> experiments();
metabolites = raw |> metabolites();

print(length(experiments));
print(length(metabolites));

pause();