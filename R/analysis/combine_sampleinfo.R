#' Combine expression matrix with sample metadata
#'
#' This function performs row-wise merging of transposed expression data with 
#' sample metadata, requiring matching row names between the two inputs.
#'
#' @param x A numeric matrix or data.frame containing expression data. 
#'   Rows represent features (genes/proteins), columns represent samples.
#' @param sampleinfo A data.frame containing sample metadata. Must include:
#'   \itemize{
#'     \item{sample_name: Character vector with sample identifiers}
#'     \item{sample_info: Factor or character vector with sample classification}
#'     \item{color: Character vector specifying display colors for samples}
#'   }
#'   Row names must match column names of the original expression matrix.
#'
#' @return A merged data.frame with:
#'   \itemize{
#'     \item{First 3 columns: sample_name, class (from sample_info), color}
#'     \item{Subsequent columns: expression values from input matrix}
#'   }
#'   Note: The input matrix is transposed before merging (samples become rows).
#'
#' @examples
#' # Create example data
#' expr_matrix <- matrix(rnorm(20), ncol=4, 
#'                       dimnames=list(paste0("Gene",1:5), paste0("S",1:4)))
#' sample_data <- data.frame(
#'   sample_name = paste0("Sample",1:4),
#'   sample_info = c("Control", "Control", "Treatment", "Treatment"),
#'   color = c("#1b9e77", "#1b9e77", "#d95f02", "#d95f02"),
#'   row.names = paste0("S",1:4)
#' )
#'
#' # Combine datasets
#' combined <- combine_sampleinfo(expr_matrix, sample_data)
#' head(combined[, 1:5])
#' 
#' @note Critical requirements:
#' 1. Original matrix column names must match sampleinfo row names
#' 2. sampleinfo must contain specified columns (sample_name, sample_info, color)
#' 3. Function will coerce both inputs to data.frame format
#' 
#' @seealso \code{\link{merge}} for general data merging functionality
#' @export
const combine_sampleinfo = function(x, sampleinfo) {
    x          <- coerce_dataframe(x);
    sampleinfo <- coerce_dataframe(sampleinfo); 

    print("view of the sampleinfo data:");
    print(sampleinfo, max.print = 6);

    x <- t(x);

    # assign sample name and sample class
    sampleinfo <- sampleinfo[rownames(x), ];
    sampleinfo <- data.frame(sample_name = sampleinfo$sample_name,
        class = sampleinfo$sample_info,
        color = sampleinfo$color,
        row.names = rownames(sampleinfo));
    # combine the expression matrix with sampleinfo
    x <- cbind(sampleinfo, x);

    x;
}