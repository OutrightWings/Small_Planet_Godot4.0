//https://godotshaders.com/shader/decal-shader-4-0-port/
shader_type spatial;
render_mode world_vertex_coords, unshaded,cull_disabled;
uniform vec4 albedo : source_color;
//uniform sampler2D texture_albedo : source_color;
uniform sampler2D DEPTH_TEXTURE: hint_depth_texture;
uniform float cube_half_x = 1.0;
uniform float cube_half_y = 1.0;
uniform float cube_half_z = 1.0;
uniform float outline_thickness = 0.2;
varying mat4 INV_MODEL_MATRIX;

void vertex(){
	INV_MODEL_MATRIX = inverse(MODEL_MATRIX);
}

// Credit: https://stackoverflow.com/questions/32227283/getting-world-position-from-depth-buffer-value
vec3 world_pos_from_depth(float depth, vec2 screen_uv, mat4 inverse_proj, mat4 inverse_view) {
	float z = depth;
	
	vec4 clipSpacePosition = vec4(screen_uv * 2.0 - 1.0, z, 1.0);
	vec4 viewSpacePosition = inverse_proj * clipSpacePosition;
	
	viewSpacePosition /= viewSpacePosition.w;
	
	vec4 worldSpacePosition = inverse_view * viewSpacePosition;
	
	return worldSpacePosition.xyz;
}

void fragment() {
	float depth = texture(DEPTH_TEXTURE, SCREEN_UV).x;
	vec3 world_pos = world_pos_from_depth(depth, SCREEN_UV, INV_PROJECTION_MATRIX, (INV_VIEW_MATRIX));
	vec4 test_pos = (INV_MODEL_MATRIX * vec4(world_pos, 1.0));
	
	if ((abs(test_pos.x) <= cube_half_x) && (abs(test_pos.y) <= cube_half_y) && (abs(test_pos.z) <= cube_half_z) &&
		(abs(test_pos.x) >= cube_half_x-outline_thickness || abs(test_pos.z) >= cube_half_z-outline_thickness)){
		ALBEDO = albedo.rgb;//texture(texture_albedo, (test_pos.xz) + 0.5).rgb * albedo.rgb;
		ALPHA = albedo.a;//texture(texture_albedo, (test_pos.xz) + 0.5).a * albedo.a;
	} else{
		discard;
	}
}

