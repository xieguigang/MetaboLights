// export R# package module type define for javascript/typescript language
//
//    imports "repository" from "MetaboLights";
//
// ref=MetaboLights.Rscript@MetaboLights, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
*/
declare namespace repository {
   module as {
      /**
        * @param env default value Is ``null``.
      */
      function metaSet(database: any, env?: object): object;
   }
   /**
     * @param env default value Is ``null``.
   */
   function experiments(database: any, env?: object): object;
   /**
     * @param ignoreCase default value Is ``true``.
     * @param env default value Is ``null``.
   */
   function keywordFilters(studies: any, keywords: string, ignoreCase?: boolean, env?: object): any;
   /**
   */
   function loadMetaEntries(file: string): object;
   /**
     * @param env default value Is ``null``.
   */
   function metabolites(database: any, env?: object): object;
   /**
   */
   function parseChEBIEntity(xml: string): object;
}
