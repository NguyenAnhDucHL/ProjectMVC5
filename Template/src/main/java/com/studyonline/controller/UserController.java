package com.studyonline.controller;


import com.studyonline.entity.User;
import com.studyonline.service.UserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;

import javax.servlet.http.HttpServletRequest;
import java.io.BufferedOutputStream;
import java.io.File;
import java.io.FileOutputStream;
import java.util.ArrayList;
import java.util.List;
import java.util.Optional;


@Controller
public class UserController {


    @Autowired
    private UserService userService;

    @RequestMapping("/user-list")
    public String userList(Model model){
        List<User> users = userService.getAllUser();
        model.addAttribute("user_list",users);
        return "CMS/user-list";
    }

    @RequestMapping("/user-details")
    public String addUser(Model model)
    {
       model.addAttribute("user", new User());
       return "CMS/user-details";
    }

    @GetMapping("/block-user/{userId}")
    public String blockUser(Model model, @PathVariable("userId") int id) {
        userService.blockUser(id);

        return "redirect:/user-list";
    }

    @RequestMapping(value = "/uploadOneFile", method = RequestMethod.POST)
    public String uploadOneFileHandlerPOST(HttpServletRequest request, //
                                           Model model, //
                                           @ModelAttribute("myUploadForm") User myUploadForm) {

        return this.doUpload(request, model, myUploadForm);

    }

    private String doUpload(HttpServletRequest request, Model model, //
                            User myUploadForm) {

        String description = myUploadForm.getPicture();
        System.out.println("Description: " + description);

        // Root Directory.
        String uploadRootPath = request.getServletContext().getRealPath("upload");
        System.out.println("uploadRootPath=" + uploadRootPath);

        File uploadRootDir = new File(uploadRootPath);
        // Create directory if it not exists.
        if (!uploadRootDir.exists()) {
            uploadRootDir.mkdirs();
        }
        MultipartFile[] fileDatas = myUploadForm.getFileDatas();
        //
        List<File> uploadedFiles = new ArrayList<File>();
        List<String> failedFiles = new ArrayList<String>();

        for (MultipartFile fileData : fileDatas) {

            // Client File Name
            String name = fileData.getOriginalFilename();
            System.out.println("Client File Name = " + name);

            if (name != null && name.length() > 0) {
                try {
                    // Create the file at server
                    File serverFile = new File(uploadRootDir.getAbsolutePath() + File.separator + name);

                    BufferedOutputStream stream = new BufferedOutputStream(new FileOutputStream(serverFile));
                    stream.write(fileData.getBytes());
                    stream.close();
                    //
                    uploadedFiles.add(serverFile);
                    System.out.println("Write file: " + serverFile);
                } catch (Exception e) {
                    System.out.println("Error Write file: " + name);
                    failedFiles.add(name);
                }
            }
        }
        model.addAttribute("description", description);
        model.addAttribute("uploadedFiles", uploadedFiles);
        model.addAttribute("failedFiles", failedFiles);
        return "redirect:/user-list";
    }

    @RequestMapping(value = "save", method = RequestMethod.POST)
    public String save(User user) {
        userService.saveUser(user);
        return "redirect:/user-list";
    }

    @RequestMapping(value = "/edit", method = RequestMethod.GET)
    public String editUser(@RequestParam("id") Integer userId, Model model) {
        Optional<User> userEdit = userService.findUserById(userId);
        userEdit.ifPresent(user -> model.addAttribute("user", user));
        return "CMS/user-details";
    }

//        @GetMapping(path = "/user-list")
//        public JSONObject userList() {
//        return new JSONObject("{'id':'abc' }");
//    }
    @RequestMapping("/post-list")
    public String postList(){

        return "CMS/post-list";
    }
    @RequestMapping("/slide-list")
    public String slideList(){

        return "CMS/slide-list";
    }
    @RequestMapping("/exams-list")
    public String examsList(){

        return "CMS/exams-list";
    }
    @RequestMapping("/subject-list")
    public String subjectList(){

        return "CMS/subject-list";
    }
    @RequestMapping("/test-list")
    public String testList(){

        return "CMS/test-list";
    }
    @RequestMapping("/settings-list")
    public String settingsList(){

        return "CMS/settings-list";
    }
    @RequestMapping("/menu-list")
    public String menuList(){

        return "CMS/menu-list";
    }
    @RequestMapping("/permissions-list")
    public String permissionsList(){

        return "CMS/permissions-list";
    }
    @RequestMapping("/roles-menu")
    public String rolesMenu(){

        return "CMS/roles-menu";
    }
    @RequestMapping("/roles-permission")
    public String rolesPermission(){

        return "CMS/roles-permission";
    }
    @RequestMapping("/registrations-list")
    public String registrationList(){

        return "CMS/registrations-list";
    }
    @RequestMapping("/course-list")
    public String courseList(){

        return "CMS/course-list";
    }
    @RequestMapping("/course-works")
    public String courseWorks(){

        return "CMS/course-works";
    }
    @RequestMapping("/pratice-result")
    public String praticeResult(){

        return "CMS/pratice-result";
    }
    @RequestMapping("/")
    public String homePage(){
        return "CMS/index";
    }
}
