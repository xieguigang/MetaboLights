# Load the MetaboAnalystR package
library("MetaboAnalystR");

local_peaks = "peaktable.csv";

## Data Filtering and Normalization
mSet <- InitDataObjects("pktable", "stat", FALSE);
mSet <- Read.TextData(mSet, local_peaks, "col", "disc")
mSet <- SanityCheckData(mSet)
mSet <- ReplaceMin(mSet);
mSet <- FilterVariable(mSet, "iqr", "F", 25)
mSet <- PreparePrenormData(mSet)
mSet <- Normalization(mSet, "MedianNorm", "LogNorm", "NULL", ratio=FALSE, ratioNum=20)