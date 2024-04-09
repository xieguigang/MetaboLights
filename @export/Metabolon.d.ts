// export R# package module type define for javascript/typescript language
//
//    imports "Metabolon" from "MetaboLights";
//
// ref=MetaboLights.MetabolonPathmap@MetaboLights, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

/**
*/
declare namespace Metabolon {
   module as {
      /**
      */
      function render(network: object, association: object): object;
   }
   /**
     * @param env default value Is ``null``.
   */
   function highlights(render: object, highlight: object, env?: object): object;
   /**
   */
   function matches_cas(cas: string, association: object): any;
}
