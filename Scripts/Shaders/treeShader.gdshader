shader_type spatial;

global uniform sampler2D treeHeight;

void vertex(){
    //float scale = texture(treeHeight,VERTEX.xz).a;
    float scale = INSTANCE_CUSTOM.a;
    VERTEX.y *= scale;
    VERTEX.x *= scale;
    VERTEX.z *= scale;
}
void fragment(){
    vec4 color = vec4(19.0/255.0, 153.0/255.0, 12.0/255.0, 1.0); // white color
    ALBEDO = color.rgb;
}