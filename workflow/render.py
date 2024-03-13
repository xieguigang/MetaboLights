import r_lambda
import os

from r_lambda.docker import docker_image

# pip3 install r_lambda

def render_metabolon(highlights, outputdir = "./", default_fill = "lightgray", 
                     image = "mzkit:metabolights_20240313-release",
                     run_debug = False):

    outputdir = os.path.abspath(outputdir)
    argv = {
        "outputdir": outputdir, 
        "highlights": highlights, 
        "default.fill_color": default_fill
    }

    return r_lambda.call_lambda(
        "MetaboLights::pathmaps",
        argv=argv,
        options={"cache.enable": True},
        workdir=outputdir,
        docker=docker_image(id=image, volumn=["outputdir"], name=None, tty = False),
        run_debug=run_debug
    )

if __name__ == "__main__":

    render_metabolon(highlights = {
        "HMDB0002820": "red",
        "6505-45-9": "green",
        "HMDB0094656": "blue",
        "HMDB0000056": "blue",
        "535-83-1": "blue",
        "24003-67-6": "blue",
        "1005-24-9": "blue"
    })    
