import {computed} from "vue";

export function useIconSizeMapping(size: String){
    const width = computed(() =>{
        switch (size){
            case "small":
                return 22;
            case "normal":
            default:
                return 40;
        }
    })

    const height = computed(() =>{
        switch (size){
            case "small":
                return 22;
            case "normal":
            default:
                return 40;
        }
    })
    
    return {
        width, height
    }
}