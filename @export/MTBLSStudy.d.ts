// export R# package module type define for javascript/typescript language
//
//    imports "MTBLSStudy" from "MetaboLights";
//
// ref=MetaboLights.MTBLSStudy@MetaboLights, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
 * MTBLS study project data reader
 * 
*/
declare namespace MTBLSStudy {
   module read {
      /**
        * @param env default value Is ``null``.
      */
      function study_source(file: any, env?: object): object;
   }
}
