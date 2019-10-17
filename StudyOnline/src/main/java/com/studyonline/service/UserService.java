package com.studyonline.service;

import com.studyonline.entity.User;

import java.util.List;
import java.util.Optional;

public interface UserService {
    List<User> getAllUser();

    void saveUser(User user);

    Optional<User>findUserById(Integer id);
    User blockUser(int id);
}
