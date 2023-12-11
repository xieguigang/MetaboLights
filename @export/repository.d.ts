// export R# package module type define for javascript/typescript language
//
//    imports "repository" from "MetaboLights";
//
// ref=MetaboLights.Rscript@MetaboLights, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * 
*/
declare namespace repository {
   module as {
      /**
       * Convert as dataset collection
       * 
       * 
        * @param database -
        * @param env -
        * 
        * + default value Is ``null``.
      */
      function metaSet(database: any, env?: object): object;
   }
   /**
    * get research study data from the database repository
    * 
    * 
     * @param database -
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function experiments(database: any, env?: object): object;
   /**
    * filter the studies collection with a given set of the keywords
    * 
    * 
     * @param studies -
     * @param keywords -
     * @param ignoreCase -
     * 
     * + default value Is ``true``.
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function keywordFilters(studies: any, keywords: string, ignoreCase?: boolean, env?: object): any;
   /**
    * load the complete metabolights database
    * 
    * > the database file could be download from the url link: 
    * >  http://ftp.ebi.ac.uk/pub/databases/metabolights/eb-eye/eb-eye_metabolights_complete.xml
    * 
     * @param file should be a file path to the complete metabolights 
     *  database file: ``eb-eye_metabolights_complete.xml``
   */
   function loadMetaEntries(file: string): object;
   /**
    * get metabolites data from the database repository
    * 
    * 
     * @param database -
     * @param env -
     * 
     * + default value Is ``null``.
   */
   function metabolites(database: any, env?: object): object;
   /**
   */
   function parseChEBIEntity(xml: string): object;
}
