# YOLOX with built-in Post Processing using HoloLab DNN Packages

## About

This Unity project is sample app of YOLOX with built-in post processing using HoloLab DNN Packages.  

HoloLab DNN Packages implements object detection class with YOLOX model. However, it doesn't support for YOLOX model with built-in post processing.  
You can learn how to implement new inference classes for such models that have not been implemented in HoloLab DNN Packages.  

## YOLOX with built-in Post Processing

This sample includes the pre-trained model of 426_YOLOX-Body-Head-Hand from PINTO Model Zoo.  
This model detects the head, face, hands, and body.  

In addition to it's impressive high-performance, this model also features built-in post processing.  
Therefore, It is unnecessary to implement post processing for YOLOX yourself.  

If you want more deteals, Please see PINTO Model Zoo.  

* [426_YOLOX-Body-Head-Hand | PINTO Model Zoo](https://github.com/PINTO0309/PINTO_model_zoo/tree/main/426_YOLOX-Body-Head-Hand)  

## Environment

These sample work on Unity 2021.3 LTS or later.  

## License

Copyright &copy; 2024 Tsukasa Sugiura  
Distributed under the [MIT License](https://opensource.org/license/mit/).  

## Contact

* Tsukasa Sugiura  
    * <t.sugiura0204@gmail.com>  
    * <https://sugiura-lab.hatenablog.com/>  

## Reference

* [HoloLab DNN Packages](https://github.com/HoloLabInc/HoloLabDnnPackages)
* [PINTO Model Zoo](https://github.com/PINTO0309/PINTO_model_zoo)