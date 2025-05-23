# Use Google API for data downloading peak feature data generated by FormatPeakList here. 
# Please "install.packages('googledrive')" and "install.packages('httpuv')"first.
library(googledrive);
temp <- tempfile(fileext = ".csv")
# Please authorize your google account to access the data
dl1 <- drive_download(
  as_id("1wEh2P81J_xFWJs5y4mq98-FsjxJ5wmBO"), path = temp, overwrite = TRUE)

# Use Google API for data downloading meta data here. 
# Please "install.packages('googledrive')" and "install.packages('httpuv')"first.
library(googledrive);
temp <- tempfile(fileext = ".csv")
# Please authorize your google account to access the data
dl2 <- drive_download(
  as_id("1KaBnSNRrirVPvpRxIubGCqpjX8asNeVA"), path = temp, overwrite = TRUE)

# Data preparation - read data in & transpose.
# This is a reference example for user to prepare their data. 
# Please prepare your data table according to your data format.
MetaboAna_Data <- t(read.csv(dl1$local_path,header = T));
colnames(MetaboAna_Data) <- MetaboAna_Data[1,];
MetaboAna_Data <- MetaboAna_Data[-1,];
MetaboAna_Data <- MetaboAna_Data[order(rownames(MetaboAna_Data)),];

meta_data <- read.csv(dl2$local_path);
meta_data <- meta_data[order(meta_data[,1]),c(1,2,4)];

Prepared_Data <- cbind(meta_data,MetaboAna_Data)[,-4];
write.csv(Prepared_Data,file = "IBD_BC_correction.csv",row.names = F)
datapath <- paste0(getwd(),"/IBD_BC_correction.csv")